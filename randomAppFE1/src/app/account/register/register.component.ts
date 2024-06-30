import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { LoginReqDto, RegisterReqDto } from '../models/account.model';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  password = new FormControl('', [Validators.required]);
  email = new FormControl('', [Validators.required]);
  username = new FormControl('', [Validators.required]);

  constructor(private accountService: AccountService) {}

  onRegisterClicked() {
    if (this.areFieldsValid()) {
      this.register();
    } else {
      console.log('form is not valid');
    }
  }

  private areFieldsValid() {
    return this.password.valid && this.email.valid && this.username.valid;
  }

  private register() {
    const registerReqDto: RegisterReqDto = {
      email: this.email.value?.toString() || '',
      password: this.password.value?.toString() || '',
      username: this.password.value?.toString() || '',
    };
    this.accountService.register(registerReqDto);
  }
}
