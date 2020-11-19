import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { TableVirtualScrollDataSource } from 'ng-table-virtual-scroll';
import { Observable } from 'rxjs';
import { YuleLog } from 'src/classes/yuleLogTypes';

@Component({
  selector: 'app-log-table',
  templateUrl: './log-table.component.html',
  styleUrls: ['./log-table.component.css']
})
export class LogTableComponent implements OnInit, OnChanges {

  constructor() { }

  @Input() yuleLogs: Array<YuleLog> = [];

  dataSource = new TableVirtualScrollDataSource();

  columns: string[] = ["logID", "logCategory", "logText", "logDate"];

  ngOnInit(): void {
    this.dataSource = new TableVirtualScrollDataSource(this.yuleLogs);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.yuleLogs) {
      this.dataSource = new TableVirtualScrollDataSource(this.yuleLogs);
    }
  }
}
