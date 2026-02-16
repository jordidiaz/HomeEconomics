import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { MovementsService } from "../movements-service";
import { FrequencyType } from "../../types/frequency-type";
import { MovementType } from "../../types/movement-type";
import type { Movement } from "../../types/movement";

type Deferred<T> = {
  promise: Promise<T>;
  resolve: (value: T) => void;
  reject: (reason?: Error) => void;
};

const createDeferred = <T,>(): Deferred<T> => {
  let resolve: (value: T) => void = () => undefined;
  let reject: (reason?: Error) => void = () => undefined;
  const promise = new Promise<T>((promiseResolve, promiseReject) => {
    resolve = promiseResolve;
    reject = promiseReject;
  });
  return { promise, resolve, reject };
};

type MockResponse = {
  ok: boolean;
  status: number;
  json: () => Promise<unknown>;
};

const createResponse = (options: MockResponse): MockResponse => options;

describe("MovementsService", () => {
  const fetchMock = vi.fn<
    (input: RequestInfo | URL, init?: RequestInit) => Promise<MockResponse>
  >();

  beforeEach(() => {
    vi.stubGlobal("fetch", fetchMock);
  });

  afterEach(() => {
    fetchMock.mockReset();
    vi.unstubAllGlobals();
  });

  it("getAll returns movements data", async () => {
    const movements: Movement[] = [
      {
        id: 1,
        name: "Nomina",
        amount: 1200,
        type: MovementType.Income,
        frequencyType: FrequencyType.Monthly,
        frequencyMonth: 1,
        frequencyMonths: Array(12).fill(false),
      },
    ];

    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => ({ movements }),
      }),
    );

    const result = await MovementsService.getAll();

    expect(fetchMock).toHaveBeenCalledWith("/api/movements");
    expect(result).toEqual(movements);
  });

  it("getAll throws on non-2xx responses", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: false,
        status: 500,
        json: async () => ({ message: "error" }),
      }),
    );

    await expect(MovementsService.getAll()).rejects.toThrow(
      "Failed to fetch movements (500)",
    );
  });

  it("getAll caches the in-flight request", async () => {
    const jsonDeferred = createDeferred<{ movements: Movement[] }>();
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: () => jsonDeferred.promise,
      }),
    );

    const first = MovementsService.getAll();
    const second = MovementsService.getAll();

    expect(fetchMock).toHaveBeenCalledTimes(1);

    jsonDeferred.resolve({ movements: [] });
    const [firstResult, secondResult] = await Promise.all([first, second]);
    expect(firstResult).toEqual([]);
    expect(secondResult).toEqual([]);

    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => ({ movements: [] }),
      }),
    );

    await MovementsService.getAll();
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  it("create sends POST with JSON body", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => 12,
      }),
    );

    const request = {
      name: "Internet",
      amount: 35.5,
      type: MovementType.Expense,
      frequency: {
        type: FrequencyType.Monthly,
        month: 1,
        months: Array(12).fill(false),
      },
    };

    const result = await MovementsService.create(request);

    expect(result).toBe(12);
    expect(fetchMock).toHaveBeenCalledWith("/api/movements", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });
  });

  it("update sends PUT with payload that includes the id", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 200,
        json: async () => undefined,
      }),
    );

    const request = {
      id: 99,
      name: "Seguro",
      amount: 20,
      type: MovementType.Expense,
      frequency: {
        type: FrequencyType.None,
        month: 1,
        months: Array(12).fill(false),
      },
    };

    await MovementsService.update(2, request);

    expect(fetchMock).toHaveBeenCalledWith("/api/movements/2", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ ...request, id: 2 }),
    });
  });

  it("delete sends DELETE", async () => {
    fetchMock.mockResolvedValueOnce(
      createResponse({
        ok: true,
        status: 204,
        json: async () => undefined,
      }),
    );

    await MovementsService.delete(4);

    expect(fetchMock).toHaveBeenCalledWith("/api/movements/4", {
      method: "DELETE",
    });
  });
});
