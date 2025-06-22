export default class BaseModel
{
    Id!: string;
    CreatedDate!: Date;
    ModifiedDate?: Date;
    IsDeleted!: boolean;
}