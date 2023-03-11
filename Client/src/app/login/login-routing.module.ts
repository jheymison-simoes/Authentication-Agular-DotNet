import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserNotAuthenticatedGuard } from '../core/guards/user-not-authenticated.guard';
import { LoginComponent } from './page/login.component';

const routes: Routes = [
  {path: '', component: LoginComponent, canActivate: [UserNotAuthenticatedGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoginRoutingModule { }
