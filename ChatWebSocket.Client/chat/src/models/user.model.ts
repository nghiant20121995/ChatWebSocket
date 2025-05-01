import BaseModel from "./base.model";

export default class User extends BaseModel
{
    FullName: string | undefined;
    Avatar: string  | undefined;
    PhoneNumber: string | undefined;
    Email: string | undefined;
}