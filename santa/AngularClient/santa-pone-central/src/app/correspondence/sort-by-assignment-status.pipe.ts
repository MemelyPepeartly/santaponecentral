import { Pipe, PipeTransform } from '@angular/core';
import { AssignmentStatus } from 'src/classes/client';
import { MessageHistory } from 'src/classes/message';

@Pipe({
  name: 'sortByAssignmentStatus'
})
export class SortByAssignmentStatusPipe implements PipeTransform {

  transform(value: any, assignmentStatus : AssignmentStatus): any {
    return value.filter((history: MessageHistory) => {
       return history.assignmentStatus.assignmentStatusID == assignmentStatus.assignmentStatusID;
     });
   }

}
