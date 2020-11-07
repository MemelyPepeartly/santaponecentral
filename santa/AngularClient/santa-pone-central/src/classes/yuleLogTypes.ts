export class YuleLog {
  logID: string;
  category: Category = new Category();
  logDate: Date;
  logtext: string;
}
export class Category {
  categoryID: string;
  categoryName: string;
  categoryDescription: string;
}
