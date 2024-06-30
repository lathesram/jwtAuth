import { NgModule } from '@angular/core';
import { AccountService } from './services/account.service';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';
import { MaterialModules } from '../app.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  providers: [AccountService],
  imports: [
    AccountRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    ...MaterialModules,
  ],
})
export class AccountModule {}
