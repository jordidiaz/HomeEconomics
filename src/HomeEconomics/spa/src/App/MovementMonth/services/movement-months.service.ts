import { TMovementMonth, TMonthMovement } from "../models/movement-month.models";
import http from "../../infrastructure/http";

interface createMovementMonthDTO {
  year: number;
  month: number;
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

const movementMonthsService = {
  get,
  create,
  payMonthMovement,
  unpayMonthMovement
};

export default movementMonthsService;