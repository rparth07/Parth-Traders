import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticateComponent } from './authenticate/authenticate.component';
import { HomeComponent } from './home/home.component';
import { AuthGuardService as AuthGuard } from './services/auth-guard.service';
import { LogInGuardService as LogInGuard } from './services/log-in-guard.service';

const routes: Routes = [
  {
    path: 'authenticate',
    component: AuthenticateComponent,
    canActivate: [LogInGuard],
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'home',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule { }
