import axios from "axios";
import ServerURL from "./server_url";

export const axiosInstance = axios.create({baseURL: ServerURL});