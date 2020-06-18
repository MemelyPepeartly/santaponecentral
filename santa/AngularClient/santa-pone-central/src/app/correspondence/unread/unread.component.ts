import { Component, OnInit, Input } from '@angular/core';
import { ContactTable } from 'src/app/interfaces/contact-table';

const ELEMENT_DATA: ContactTable[] = [
  {sender: 'Picky Wikket', recipient: 'Memely', event: "Gift Exchange"}
];

@Component({
  selector: 'app-unread',
  templateUrl: './unread.component.html',
  styleUrls: ['./unread.component.css']
})
export class UnreadComponent implements OnInit {

  constructor() { }

  @Input() unreadMessageColumns: string[];
  dataSource = ELEMENT_DATA;

  ngOnInit() {
  }

}
