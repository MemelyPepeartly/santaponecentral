import { Component, OnInit } from '@angular/core';



@Component({
  selector: 'app-correspondence',
  templateUrl: './correspondence.component.html',
  styleUrls: ['./correspondence.component.css']
})
export class CorrespondenceComponent implements OnInit {

  constructor() { }

  displayedColumns: string[] = ['sender', 'recipient', 'event', 'contact'];

  ngOnInit() {
  }

}
