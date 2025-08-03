export interface ResultObject<TResponse> {
    errors?: string[]
    statusCode?: number
    data?: TResponse
}

