export class BaseResponse<T> {
    Code: number = 0;
    Message?: string;
    Data?: T;

    constructor(code: number, message: string, data: T) {
        this.Code = code;
        this.Message = message;
        this.Data = data;
    }
}