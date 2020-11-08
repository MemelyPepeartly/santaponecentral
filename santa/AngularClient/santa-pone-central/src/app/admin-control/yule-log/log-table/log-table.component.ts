import { Component, Input, OnInit } from '@angular/core';
import { YuleLog } from 'src/classes/yuleLogTypes';

@Component({
  selector: 'app-log-table',
  templateUrl: './log-table.component.html',
  styleUrls: ['./log-table.component.css']
})
export class LogTableComponent implements OnInit {

  constructor() { }

  @Input() yuleLogs: Array<YuleLog> = [];

  columns: string[] = ["logID", "logCategory", "logText", "logDate"];

  ngOnInit(): void {
  }

}
