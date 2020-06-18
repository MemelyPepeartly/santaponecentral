import { Component, OnInit, Input } from '@angular/core';
import { ContactTable } from 'src/app/interfaces/contact-table';

const ELEMENT_DATA: ContactTable[] = [
  {sender: 'Parpy Treetrunk', recipient: 'Picky Wikket', event: "Card Exchange"},
  {sender: 'Nibbles', recipient: 'Lucky Lightshow', event: "Card Exchange"}
];

@Component({
  selector: 'app-pending',
  templateUrl: './pending.component.html',
  styleUrls: ['./pending.component.css']
})
export class PendingComponent implements OnInit {

  constructor() { }

  @Input() pendingMessageColumns: string[];
  dataSource = ELEMENT_DATA;

  ngOnInit(): void {
  }

}
