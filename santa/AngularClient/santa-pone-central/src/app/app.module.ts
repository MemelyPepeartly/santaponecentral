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
import { ContactComponent } from './contact/contact.component';
import { SignupComponent } from './signup/signup.component';
import { AdminnavComponent } from './navbar/adminnav/adminnav.component';
import { HeadquartersComponent } from './headquarters/headquarters.component';
import { ApprovedAnonsComponent } from './headquarters/approved-anons/approved-anons.component';
import { IncomingSignupsComponent } from './headquarters/incoming-signups/incoming-signups.component';
import { MaterialElevationDirective } from './headquarters/material-elevation.directive';
import { SignupFormComponent } from './signup/signup-form/signup-form.component';
import { AdminHelpComponent } from './admin-help/admin-help.component';
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
import { ContactPanelComponent } from './profile/contact-panel/contact-panel.component';
import { CorrespondenceComponent } from './correspondence/correspondence.component';
import { UnreadComponent } from './correspondence/unread/unread.component';
import { AllComponent } from './correspondence/all/all.component';
import { PendingComponent } from './correspondence/pending/pending.component';
import { SelectedChatComponent } from './correspondence/selected-chat/selected-chat.component';
import { SelectedRecipientComponent } from './profile/selected-recipient/selected-recipient.component';
import { ChatHistoriesComponent } from './shared/chat-histories/chat-histories.component'


@NgModule({
   declarations: [
      AppComponent,
      HomeComponent,
      ProfileComponent,
      NavbarComponent,
      UsernavComponent,
      LoginComponent,
      DefaultnavComponent,
      ContactComponent,
      SignupComponent,
      AdminnavComponent,
      HeadquartersComponent,
      ApprovedAnonsComponent,
      IncomingSignupsComponent,
      MaterialElevationDirective,
      SignupFormComponent,
      AdminHelpComponent,
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
      UnreadComponent,
      AllComponent,
      PendingComponent,
      SelectedChatComponent,
      SelectedRecipientComponent,
      ChatHistoriesComponent
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
