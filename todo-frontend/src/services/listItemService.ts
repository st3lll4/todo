import axios, { AxiosInstance, AxiosResponse } from "axios";
import { AxiosError } from "axios";
import { ResultObject } from "../types/ResultObject";
import { ListItem } from "../types/ListItem";

export class ListItemService {
  protected axiosInstance: AxiosInstance;
  private basePath : string = 'listItems/'
  constructor() {
    this.axiosInstance = axios.create({
      baseURL: "http://localhost:5066/api/",
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


  async addAsync(entity: ListItem): Promise<ResultObject<ListItem>> {
    try {
      const response = await this.axiosInstance.post<ListItem>(
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

  async updateAsync(entity: ListItem): Promise<ResultObject<ListItem>> {
    try {
      const response = await this.axiosInstance.put<ListItem>(
        this.basePath + entity.id,
        entity
      );

      return this.handleResponse(response);
    } catch (error) {
      return this.handleError(error as AxiosError);
    }
  }

}
