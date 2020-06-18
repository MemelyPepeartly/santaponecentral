import { Component, OnInit, Input } from '@angular/core';
import { ContactTable } from 'src/app/interfaces/contact-table';

const ELEMENT_DATA: ContactTable[] = [
  {sender: 'Picky Wikket', recipient: 'Memely', event: "Gift Exchange"},
  {sender: 'Memely', recipient: 'Lucky Lightshow', event: "Gift Exchange"},
  {sender: 'Memely', recipient: 'Wibble Wab', event: "Card Exchange"}
];

@Component({
  selector: 'app-all',
  templateUrl: './all.component.html',
  styleUrls: ['./all.component.css']
})
export class AllComponent implements OnInit {

  constructor() { }

  @Input() allMessageColumns: string[];
  dataSource = ELEMENT_DATA;

  ngOnInit(): void {
  }

}
