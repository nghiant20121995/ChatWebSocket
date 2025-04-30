import BaseModel from "./base.model";

export default class User extends BaseModel
{
    FullName?: string;
    Avatar?: string;
    PhoneNumber?: string;
    Email?: string;
}