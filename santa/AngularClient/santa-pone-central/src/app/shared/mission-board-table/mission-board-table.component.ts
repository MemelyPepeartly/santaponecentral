import { Component, OnInit, Input } from '@angular/core';
import { BoardEntry } from 'src/classes/missionBoards';

@Component({
  selector: 'app-mission-board-table',
  templateUrl: './mission-board-table.component.html',
  styleUrls: ['./mission-board-table.component.css']
})
export class MissionBoardTableComponent implements OnInit {

  constructor() { }

  @Input() boardEntries: Array<BoardEntry> = [];

  columns: string[] = ["number", "description", "type"];

  ngOnInit(): void {
  }
}
