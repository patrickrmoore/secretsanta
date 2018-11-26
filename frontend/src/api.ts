import axios from "axios";
import { User } from "./models";

const baseUrl = `https://secretsanta2018.azurewebsites.net/api`;

export const getAllUsers = async () => {
  return await axios.get<User[]>(`${baseUrl}/users`, {
    params: { code: "sOkMN8muXZhVtLC4Tc5PkDuRveEVLOreSX1td9SpuptEbxV1ZjpA9A==" }
  });
};

export const getUser = async (code: string) => {
  return await axios.get<User>(`${baseUrl}/users/${code}`, {
    params: { code: "sOkMN8muXZhVtLC4Tc5PkDuRveEVLOreSX1td9SpuptEbxV1ZjpA9A==" }
  });
};

export const updateUser = async (user: User) => {
  return await axios.post<User>(`${baseUrl}/users/${user.code}`, user, {
    params: { code: "sOkMN8muXZhVtLC4Tc5PkDuRveEVLOreSX1td9SpuptEbxV1ZjpA9A==" }
  });
};

export const authenticate = async (accessCode: string) => {
  return await axios.get<User>(`${baseUrl}/authenticate`, {
    params: {
      accessCode,
      code: "sOkMN8muXZhVtLC4Tc5PkDuRveEVLOreSX1td9SpuptEbxV1ZjpA9A=="
    }
  });
};
