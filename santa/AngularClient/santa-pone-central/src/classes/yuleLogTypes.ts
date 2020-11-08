export class YuleLog {
  logID: string;
  category: Category = new Category();
  logDate: Date;
  logText: string;
}
export class Category {
  categoryID: string;
  categoryName: string;
  categoryDescription: string;
}
