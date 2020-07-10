import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { SignupComponent } from './signup/signup.component';
import { ContactComponent } from './contact/contact.component';
import { HeadquartersComponent } from './headquarters/headquarters.component';
import { AdminHelpComponent } from './admin-help/admin-help.component';
import { CorrespondenceComponent } from './correspondence/correspondence.component';
import { AdminControlComponent } from './admin-control/admin-control.component';


//Auth imports
import { AuthGuard } from './auth/auth.guard'
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InterceptorService } from './auth/interceptor.service';


const appRoutes: Routes = [
  { path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'signup',
    component: SignupComponent
  },
  {
    path: 'contact',
    component: ContactComponent
  },
  {
    path: 'headquarters',
    component: HeadquartersComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'correspondence',
    component: CorrespondenceComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'help',
    component: AdminHelpComponent
  },
  {
    path: 'admin-control',
    component: AdminControlComponent,
    canActivate: [AuthGuard]
  },
  {
		path: "**",
		redirectTo: "home"
  },
  { 
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true
    }
  ]
})
export class AppRoutingModule { }
