import { Pipe, PipeTransform } from '@angular/core';
import { AssignmentStatus } from 'src/classes/client';
import { MessageHistory } from 'src/classes/message';

@Pipe({
  name: 'sortByUnreadPipe'
})
export class SortByUnreadPipePipe implements PipeTransform {

  transform(value: any): any {
    return value.filter((history: MessageHistory) => {
       return history.unreadCount > 0;
     });
   }
}
