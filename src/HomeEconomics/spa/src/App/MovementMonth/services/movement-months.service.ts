import http from "../../infrastructure/http";
import { MovementType } from "../../Movements/models/movement.models";
import { TMonthMovement, TMovementMonth } from "../models/movement-month.models";

type CreateMovementMonthDTO = {
  year: number;
  month: number;
}

type UpdateMonthMovementAmountDTO = {
  movementMonthId: number;
  monthMovementId: number;
  amount: number;
}

type AddMonthMovementDTO = {
  movementMonthId: number;
  name: string;
  amount: number;
  type: number;
}

type AddStatusDTO = {
  year: number;
  month: number;
  accountAmount: number;
  cashAmount: number;
}

const get = async (year: number, month: number): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.get<TMovementMonth>(`movement-months/${year}/${month}`);
  return response;
};

const create = async (year: number, month: number): Promise<TMovementMonth> => {
  const createMovementMonthDTO: CreateMovementMonthDTO = {
    year: year,
    month: month
  };
  const response: TMovementMonth = await http.post(`movement-months`, createMovementMonthDTO);
  return response;
};

const payMonthMovement = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}/pay`, {});
  return response;
};

const unpayMonthMovement = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}/unpay`, {});
  return response;
};

const monthMovementToNextMovementMonth = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}/to-next-movement-month`, {});
  return response;
};

const updateMonthMovementAmount = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement, newAmount: number): Promise<TMovementMonth> => {
  const updateMonthMovementAmountDTO: UpdateMonthMovementAmountDTO = {
    movementMonthId: movementMonth.id,
    monthMovementId: monthMovement.id,
    amount: newAmount
  };
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}/update-amount`, updateMonthMovementAmountDTO);
  return response;
};

const addMonthMovement = async (movementMonth: TMovementMonth, name: string, amount: number, movementType: MovementType): Promise<TMovementMonth> => {
  const addMonthMovementDTO: AddMonthMovementDTO = {
    movementMonthId: movementMonth.id,
    name: name,
    amount: amount,
    type: movementType
  };
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements`, addMonthMovementDTO);
  return response;
};

const deleteMonthMovement = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.del(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}`);
  return response;
};

const addStatus = async (movementMonth: TMovementMonth, accountAmount: number, cashAmount: number): Promise<TMovementMonth> => {
  const addStatusDTO: AddStatusDTO = {
    year: movementMonth.year,
    month: movementMonth.month,
    accountAmount: accountAmount,
    cashAmount: cashAmount
  };
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/add-status`, addStatusDTO);
  return response;
};

const movementMonthsService = {
  get,
  create,
  payMonthMovement,
  unpayMonthMovement,
  updateMonthMovementAmount,
  addMonthMovement,
  deleteMonthMovement,
  addStatus,
  monthMovementToNextMovementMonth
};

export default movementMonthsService;