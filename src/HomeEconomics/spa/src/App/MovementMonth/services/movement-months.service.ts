import { TMovementMonth, TMonthMovement } from "../models/movement-month.models";
import http from "../../infrastructure/http";

interface createMovementMonthDTO {
  year: number;
  month: number;
}

interface updateMonthMovementAmountDTO {
  movementMonthId: number;
  monthMovementId: number;
  amount: number;
}

const get = async (year: number, month: number): Promise<TMovementMonth> => {
  const response: TMovementMonth = await http.get<TMovementMonth>(`movement-months/${year}/${month}`);
  return response;
};

const create = async (year: number, month: number): Promise<TMovementMonth> => {
  const createMovementMonthDTO: createMovementMonthDTO = {
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

const updateMonthMovementAmount = async (movementMonth: TMovementMonth, monthMovement: TMonthMovement, newAmount: number): Promise<TMovementMonth> => {
  const updateMonthMovementAmountDTO: updateMonthMovementAmountDTO = {
    movementMonthId: movementMonth.id,
    monthMovementId: monthMovement.id,
    amount: newAmount
  };
  const response: TMovementMonth = await http.post(`movement-months/${movementMonth.id}/month-movements/${monthMovement.id}/update-amount`, updateMonthMovementAmountDTO);
  return response;
};

const movementMonthsService = {
  get,
  create,
  payMonthMovement,
  unpayMonthMovement,
  updateMonthMovementAmount
};

export default movementMonthsService;