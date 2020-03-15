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
import { LoaderService } from './services/loader.service';

// Interceptors
import { LoaderInterceptor } from './interceptors/loader.interceptor';

// Componenents
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './home/login/login.component';
import { DefaultnavComponent } from './navbar/defaultnav/defaultnav.component';
import { ContactComponent } from './contact/contact.component';
import { SignupComponent } from './signup/signup.component';
import { AdminnavComponent } from './navbar/adminnav/adminnav.component';
import { HeadquartersComponent } from './headquarters/headquarters.component';
import { ApprovedAnonsComponent } from './headquarters/approved-anons/approved-anons.component';
import { InTransitComponent } from './headquarters/in-transit/in-transit.component';
import { IncomingCorrespondenceComponent } from './headquarters/incoming-correspondence/incoming-correspondence.component';
import { IncomingSignupsComponent } from './headquarters/incoming-signups/incoming-signups.component';
import { ShippingCorrespondenceComponent } from './headquarters/incoming-correspondence/shipping-correspondence/shipping-correspondence.component';
import { GeneralCorrespondenceComponent } from './headquarters/incoming-correspondence/general-correspondence/general-correspondence.component';
import { MaterialElevationDirective } from './headquarters/material-elevation.directive';
import { SignupFormComponent } from './signup/signup-form/signup-form.component';
import { AdminHelpComponent } from './admin-help/admin-help.component';
import { ApinavComponent } from './navbar/apinav/apinav.component';
import { ApiComponent } from './api/api.component';
import { UserComponent } from './api/user/user.component';
import { EventComponent } from './api/event/event.component';
import { SurveyComponent } from './api/survey/survey.component';
import { QuestionComponent } from './api/question/question.component';
import { ResponseComponent } from './api/response/response.component';
import { LoaderComponent } from './shared/loader/loader.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    NotificationsComponent,
    NavbarComponent,
    UsernavComponent,
    LoginComponent,
    DefaultnavComponent,
    ContactComponent,
    SignupComponent,
    AdminnavComponent,
    HeadquartersComponent,
    ApprovedAnonsComponent,
    InTransitComponent,
    IncomingCorrespondenceComponent,
    IncomingSignupsComponent,
    ShippingCorrespondenceComponent,
    GeneralCorrespondenceComponent,
    MaterialElevationDirective,
    SignupFormComponent,
    AdminHelpComponent,
    ApinavComponent,
    ApiComponent,
    UserComponent,
    EventComponent,
    SurveyComponent,
    QuestionComponent,
    ResponseComponent,
    LoaderComponent
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
  providers: [
    LoaderService,
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
