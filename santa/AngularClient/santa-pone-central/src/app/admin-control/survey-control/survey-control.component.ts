import { Component, OnInit, Input } from '@angular/core';
import { Survey } from 'src/classes/survey';

@Component({
  selector: 'app-survey-control',
  templateUrl: './survey-control.component.html',
  styleUrls: ['./survey-control.component.css']
})
export class SurveyControlComponent implements OnInit {

  constructor() { }

  @Input() allSurveys: Array<Survey> = []
  
  ngOnInit(): void {
  }

}
