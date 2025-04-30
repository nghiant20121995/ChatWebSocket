import { BaseResponse } from './base-response.model';

export class LoginResponse {
    token: string;
    id: string;
    username: string;
    email: string;

    constructor(token: string, id: string, username: string, email: string) {
        this.token = token;
        this.id = id;
        this.username = username;
        this.email = email;
    }
}

export type LoginResponseBase = BaseResponse<LoginResponse>;