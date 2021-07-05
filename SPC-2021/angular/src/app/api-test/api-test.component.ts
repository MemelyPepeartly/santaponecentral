import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Survey } from 'src/classes/survey';
import { environment } from 'src/environments/environment';
import { SurveyService } from '../services/api services/survey.service';

@Component({
  selector: 'app-api-test',
  templateUrl: './api-test.component.html',
  styleUrls: ['./api-test.component.css']
})
export class ApiTestComponent implements OnInit {

  constructor(private surveyService: SurveyService, private http: HttpClient) { }

  public surveys: Array<any> = [];

  ngOnInit(): void {
  }
  public getSurveys()
  {
    this.surveyService.getAllSurveys().subscribe(res => {
      this.surveys = res;
    });
  }
  public getTestAPIClaims()
  {
    console.log(environment.auth);
    
    this.http.get<any>("http://localhost:3010/api/claims").subscribe(res => {
      console.log(res);
    })
  }
}
