import { Component, OnInit, Input } from '@angular/core';
import { Profile } from 'src/classes/profile';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor() { }

  @Input() profile: Profile;

  ngOnInit(): void {
  }

}
