import { BaseResponse } from './base-response.model';

export class LoginResponse {
    Token!: string;
    Id!: string;
    Username!: string;
    Email!: string;

    constructor(token: string, id: string, username: string, email: string) {
        this.Token = token;
        this.Id = id;
        this.Username = username;
        this.Email = email;
    }
}

export type LoginResponseBase = BaseResponse<LoginResponse>;