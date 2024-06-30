import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { LoginReqDto } from '../models/account.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  password = new FormControl('', [Validators.required]);
  email = new FormControl('', [Validators.required]);

  constructor(private accountService: AccountService) {}

  onLoginClicked() {
    if (this.areFieldsValid()) {
      this.login();
    } else {
      console.log('form is not valid');
    }
  }

  private areFieldsValid() {
    return this.password.valid && this.email.valid;
  }

  private login() {
    const loginReqDto: LoginReqDto = {
      email: this.email.value?.toString() || '',
      password: this.password.value?.toString() || '',
    };
    this.accountService.login(loginReqDto);
  }
}
