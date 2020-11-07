import { Component, Input, OnInit } from '@angular/core';
import { Category, YuleLog } from '../../../classes/yuleLogTypes'

@Component({
  selector: 'app-yule-log',
  templateUrl: './yule-log.component.html',
  styleUrls: ['./yule-log.component.css']
})
export class YuleLogComponent implements OnInit {

  constructor() { }

  @Input() categories: Array<Category> = [];

  ngOnInit(): void {
  }

}
