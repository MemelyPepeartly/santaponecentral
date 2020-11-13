// Necessary
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

// Extras
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CustomMaterialModule } from './core/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

// Services

// Componenents
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { NavbarComponent } from './navbar/navbar.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './home/login/login.component';
import { DefaultnavComponent } from './navbar/defaultnav/defaultnav.component';
import { SignupComponent } from './signup/signup.component';
import { AdminnavComponent } from './navbar/adminnav/adminnav.component';
import { HeadquartersComponent } from './headquarters/headquarters.component';
import { ApprovedAnonsComponent } from './headquarters/approved-anons/approved-anons.component';
import { IncomingSignupsComponent } from './headquarters/incoming-signups/incoming-signups.component';
import { MaterialElevationDirective } from './headquarters/material-elevation.directive';
import { SignupFormComponent } from './signup/signup-form/signup-form.component';
import { LoadingSpinComponent } from './shared/loading-spin/loading-spin.component';
import { SmallLoadingSpinComponent } from './shared/small-loading-spin/small-loading-spin.component';
import { SelectedAnonComponent } from './headquarters/selected-anon/selected-anon.component';
import { SurveyFormComponent } from './signup/survey-form/survey-form.component';
import { AdminControlComponent } from './admin-control/admin-control.component';
import { TagControlComponent } from './admin-control/tag-control/tag-control.component';
import { QuestionControlComponent } from './admin-control/question-control/question-control.component';
import { OptionControlComponent } from './admin-control/question-control/option-control/option-control.component';
import { EventControlComponent } from './admin-control/event-control/event-control.component';
import { SurveyControlComponent } from './admin-control/survey-control/survey-control.component';
import { ControlPanelComponent } from './profile/control-panel/control-panel.component';
import { InformationComponent } from './profile/information/information.component';
import { CorrespondenceComponent } from './correspondence/correspondence.component';
import { SelectedRecipientComponent } from './profile/selected-recipient/selected-recipient.component';
import { ChatHistoriesComponent } from './shared/chat-histories/chat-histories.component'
import { ContactPanelComponent } from './shared/contact-panel/contact-panel.component';
import { InputControlComponent } from './shared/input-control/input-control.component';
import { DeniedAnonsComponent } from './headquarters/denied-anons/denied-anons.component';
import { CompletedAnonsComponent } from './headquarters/completed-anons/completed-anons.component';
import { MessageControlPanelComponent } from './shared/message-control-panel/message-control-panel.component';
import { AutoAssignmentComponent } from './admin-control/auto-assignment/auto-assignment.component';
import { MissionBoardsComponent } from './mission-boards/mission-boards.component';
import { MissionBoardTableComponent } from './shared/mission-board-table/mission-board-table.component';
import { ManualAddComponent } from './headquarters/manual-add/manual-add.component';
import { ResponseListComponent } from './shared/response-list/response-list.component';
import { AssignmentStatusControllerComponent } from './shared/assignment-status-controller/assignment-status-controller.component';
import { AssignmentsPanelComponent } from './headquarters/selected-anon/assignments-panel/assignments-panel.component';
import { AgentCatalogueComponent } from './agent-catalogue/agent-catalogue.component';
import { AgentPairingControlComponent } from './admin-control/auto-assignment/agent-pairing-control/agent-pairing-control.component';
import { NoteControlComponent } from './headquarters/selected-anon/note-control/note-control.component';
import { ClientNoteInfoComponent } from './headquarters/client-note-info/client-note-info.component';
import { ClientAssignmentInfoComponent } from './headquarters/client-assignment-info/client-assignment-info.component';
import { UnauthorizedComponent } from './error-pages/unauthorized/unauthorized.component';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { StatusCheckerComponent } from './status-checker/status-checker.component';
import { YuleLogComponent } from './admin-control/yule-log/yule-log.component';
import { LogTableComponent } from './admin-control/yule-log/log-table/log-table.component';
import { ClientItemComponent } from './headquarters/client-item/client-item.component';


@NgModule({
   declarations: [
      AppComponent,
      HomeComponent,
      ProfileComponent,
      NavbarComponent,
      UsernavComponent,
      LoginComponent,
      DefaultnavComponent,
      SignupComponent,
      AdminnavComponent,
      HeadquartersComponent,
      ApprovedAnonsComponent,
      IncomingSignupsComponent,
      MaterialElevationDirective,
      SignupFormComponent,
      LoadingSpinComponent,
      SelectedAnonComponent,
      SmallLoadingSpinComponent,
      SurveyFormComponent,
      AdminControlComponent,
      TagControlComponent,
      QuestionControlComponent,
      OptionControlComponent,
      EventControlComponent,
      SurveyControlComponent,
      ControlPanelComponent,
      InformationComponent,
      ContactPanelComponent,
      CorrespondenceComponent,
      SelectedRecipientComponent,
      ChatHistoriesComponent,
      InputControlComponent,
      DeniedAnonsComponent,
      CompletedAnonsComponent,
      MessageControlPanelComponent,
      AutoAssignmentComponent,
      MissionBoardsComponent,
      MissionBoardTableComponent,
      ManualAddComponent,
      ResponseListComponent,
      AssignmentStatusControllerComponent,
      AssignmentsPanelComponent,
      AgentCatalogueComponent,
      AgentPairingControlComponent,
      NoteControlComponent,
      ClientNoteInfoComponent,
      ClientAssignmentInfoComponent,
      UnauthorizedComponent,
      NotFoundComponent,
      StatusCheckerComponent,
      YuleLogComponent,
      LogTableComponent,
      ClientItemComponent
   ],
   entryComponents: [
      LoginComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      CustomMaterialModule,
      FormsModule,
      ReactiveFormsModule,
      AppRoutingModule,
      RouterModule,
      HttpClientModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
