import axios, { AxiosInstance, AxiosResponse } from "axios";
import { AxiosError } from "axios";
import { ResultObject } from "@/types/ResultObject";
import { TaskList } from "@/types/TaskList";

export class TaskListService {
  protected axiosInstance: AxiosInstance;
  private basePath: string = 'taskLists/'

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api',
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
  }

  handleResponse(response: AxiosResponse) {
    if (response.status <= 300) {
      return {
        statusCode: response.status,
        data: response.data,
      };
    }

    return {
      statusCode: response.status,
      errors: [(response.status.toString() + " " + response.statusText).trim()],
    };
  }

  handleError(error: AxiosError) {
    console.log("error: ", error.message);
    return {
      statusCode: error.status ?? 0,
      errors: [error.code ?? "???"],
    };
  }

  async getAllAsync(): Promise<ResultObject<TaskList[]>> {
    try {
      const response = await this.axiosInstance.get<TaskList[]>(this.basePath);
      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }

  async getAsync(id: string): Promise<ResultObject<TaskList>> {
    try {
      const response = await this.axiosInstance.get<TaskList>(
        this.basePath + id
      );
      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }

  async addAsync(entity: TaskList): Promise<ResultObject<TaskList>> {
    try {
      const response = await this.axiosInstance.post<TaskList>(
        this.basePath,
        entity
      );
      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }

  async deleteAsync(id: string): Promise<ResultObject<null>> {
    try {
      const response = await this.axiosInstance.delete<null>(this.basePath + id);

      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }

  async updateAsync(entity: TaskList): Promise<ResultObject<TaskList>> {
    try {
      const response = await this.axiosInstance.put<TaskList>(
        this.basePath + entity.id,
        entity
      );

      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }
}
