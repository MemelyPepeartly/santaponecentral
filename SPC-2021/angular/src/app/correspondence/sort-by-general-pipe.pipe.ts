import { Pipe, PipeTransform } from '@angular/core';
import { MessageHistory } from 'src/classes/message';

@Pipe({
  name: 'sortByGeneralPipe'
})
export class SortByGeneralPipePipe implements PipeTransform {

  transform(value: any): any {
    return value.filter((history: MessageHistory) => {
       return history.relationXrefID == null;
     });
   }

}
