import { Component, OnInit, Input } from '@angular/core';
import { Question } from 'src/classes/survey';

@Component({
  selector: 'app-question-control',
  templateUrl: './question-control.component.html',
  styleUrls: ['./question-control.component.css']
})
export class QuestionControlComponent implements OnInit {

  constructor() { }

  @Input() allQuestions: Array<Question> = [];

  ngOnInit(): void {
  }

}
