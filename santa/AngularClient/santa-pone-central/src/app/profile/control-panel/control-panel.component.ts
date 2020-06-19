import { Component, OnInit, Input } from '@angular/core';
import { Client } from 'src/classes/client';
import { ProfileRecipient } from 'src/classes/profile';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-control-panel',
  templateUrl: './control-panel.component.html',
  styleUrls: ['./control-panel.component.css']
})
export class ControlPanelComponent implements OnInit {

  constructor(private gatherer: GathererService) { }

  @Input() recipients: Array<ProfileRecipient> = []
  columns: string[] = ["recipient", "event", "contact"];

  ngOnInit(): void {
  }

}
