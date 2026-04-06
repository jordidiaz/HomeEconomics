import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { MovementMonthsService } from "../movement-months-service";
import { MovementType } from "../../types/movement-type";
import type { MovementMonth } from "../../types/movement-month";

type MockResponse = {
  ok: boolean;
  status: number;
  json: () => Promise<unknown>;
};

const createResponse = (options: MockResponse): MockResponse => options;

describe("MovementMonthsService", () => {
  const fetchMock = vi.fn<
    (input: RequestInfo | URL, init?: RequestInit) => Promise<MockResponse>
  >();

  const movementMonth: MovementMonth = {
    id: 10,
    year: 2024,
    month: 5,
    nextMovementMonthExists: false,
    status: {
      pendingTotalExpenses: 0,
      pendingTotalIncomes: 0,
      accountAmount: 0,
      cashAmount: 0,
    },
    monthMovements: [],
  };

  beforeEach(() => {
    vi.stubGlobal("fetch", fetchMock);
  });

  afterEach(() => {
    fetchMock.mockReset();
    vi.unstubAllGlobals();
  });

  it("create posts payload and returns data", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => movementMonth,
      }),
    );

    const result = await MovementMonthsService.create(2024, 5);

    expect(fetchMock).toHaveBeenCalledWith("/api/movement-months", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ year: 2024, month: 5 }),
    });
    expect(result).toEqual(movementMonth);
  });

  it("getByYearMonth caches in-flight requests by key", async () => {
    let resolveJson: (value: MovementMonth) => void = () => undefined;
    const jsonPromise = new Promise<MovementMonth>((resolve) => {
      resolveJson = resolve;
    });

    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: () => jsonPromise,
      }),
    );

    const first = MovementMonthsService.getByYearMonth(2024, 5);
    const second = MovementMonthsService.getByYearMonth(2024, 5);

    expect(fetchMock).toHaveBeenCalledTimes(1);

    resolveJson(movementMonth);
    await Promise.all([first, second]);

    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => movementMonth,
      }),
    );

    await MovementMonthsService.getByYearMonth(2024, 5);
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  it("getByYearMonth throws with status on error", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: false,
        status: 404,
        json: async () => ({ message: "error" }),
      }),
    );

    await expect(MovementMonthsService.getByYearMonth(2024, 5)).rejects.toMatchObject({
      status: 404,
      message: "Failed to fetch movement month (404)",
    });
  });

  it("addMonthMovement posts payload", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => movementMonth,
      }),
    );

    await MovementMonthsService.addMonthMovement(10, {
      name: "Extra",
      amount: 12,
      type: MovementType.Income,
    });

    expect(fetchMock).toHaveBeenCalledWith("/api/movement-months/10/month-movements", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        movementMonthId: 10,
        name: "Extra",
        amount: 12,
        type: MovementType.Income,
      }),
    });
  });

  it("pay and unpay post to the correct endpoints", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.payMonthMovement(10, 7);
    await MovementMonthsService.unpayMonthMovement(10, 7);

    expect(fetchMock).toHaveBeenNthCalledWith(
      1,
      "/api/movement-months/10/month-movements/7/pay",
      { method: "POST" },
    );
    expect(fetchMock).toHaveBeenNthCalledWith(
      2,
      "/api/movement-months/10/month-movements/7/unpay",
      { method: "POST" },
    );
  });

  it("updateMonthMovementAmount posts payload", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.updateMonthMovementAmount(10, 7, 45.5);

    expect(fetchMock).toHaveBeenCalledWith(
      "/api/movement-months/10/month-movements/7/update-amount",
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ movementMonthId: 10, monthMovementId: 7, amount: 45.5 }),
      },
    );
  });

  it("updateMonthMovement posts payload", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.updateMonthMovement(10, 7, {
      name: "Luz actualizada",
      amount: 55,
      type: MovementType.Expense,
    });

    expect(fetchMock).toHaveBeenCalledWith(
      "/api/movement-months/10/month-movements/7/update",
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          movementMonthId: 10,
          monthMovementId: 7,
          name: "Luz actualizada",
          amount: 55,
          type: MovementType.Expense,
        }),
      },
    );
  });

  it("updateMonthMovement throws on non-2xx", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: false,
        status: 409,
        json: async () => undefined,
      }),
    );

    await expect(
      MovementMonthsService.updateMonthMovement(10, 7, {
        name: "Test",
        amount: 10,
        type: MovementType.Expense,
      }),
    ).rejects.toMatchObject({
      status: 409,
      message: "Failed to update month movement (409)",
    });
  });

  it("deleteMonthMovement sends DELETE", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 204,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.deleteMonthMovement(10, 7);

    expect(fetchMock).toHaveBeenCalledWith(
      "/api/movement-months/10/month-movements/7",
      { method: "DELETE" },
    );
  });

  it("moveMonthMovementToNextMonth posts to endpoint", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.moveMonthMovementToNextMonth(10, 7);

    expect(fetchMock).toHaveBeenCalledWith(
      "/api/movement-months/10/month-movements/7/to-next-movement-month",
      { method: "POST" },
    );
  });

  it("star and unstar post to the correct endpoints", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({ ok: true, status: 200, json: async () => undefined }),
    );
    fetchMock.mockResolvedValueOnce(
      createResponse({ ok: true, status: 200, json: async () => undefined }),
    );

    await MovementMonthsService.starMonthMovement(10, 7);
    await MovementMonthsService.unstarMonthMovement(10, 7);

    expect(fetchMock).toHaveBeenNthCalledWith(
      1,
      "/api/movement-months/10/month-movements/7/star",
      { method: "POST" },
    );
    expect(fetchMock).toHaveBeenNthCalledWith(
      2,
      "/api/movement-months/10/month-movements/7/unstar",
      { method: "POST" },
    );
  });

  it("starMonthMovement throws on non-2xx", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({ ok: false, status: 409, json: async () => undefined }),
    );

    await expect(MovementMonthsService.starMonthMovement(10, 7)).rejects.toMatchObject({
      status: 409,
      message: "Failed to star month movement (409)",
    });
  });

  it("unstarMonthMovement throws on non-2xx", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({ ok: false, status: 409, json: async () => undefined }),
    );

    await expect(MovementMonthsService.unstarMonthMovement(10, 7)).rejects.toMatchObject({
      status: 409,
      message: "Failed to unstar month movement (409)",
    });
  });

  it("addStatus posts payload", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    await MovementMonthsService.addStatus(10, {
      year: 2024,
      month: 5,
      accountAmount: 12,
      cashAmount: 4,
    });

    expect(fetchMock).toHaveBeenCalledWith("/api/movement-months/10/add-status", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        year: 2024,
        month: 5,
        accountAmount: 12,
        cashAmount: 4,
      }),
    });
  });
});
