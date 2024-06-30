import { Component } from '@angular/core';
import { LoginReqDto } from './account/models/account.model';
import { AccountService } from './account/services/account.service';
import { TestService } from './test.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'randomAppFE1';

  constructor(
    private accountService: AccountService,
    private testApiService: TestService
  ) {}

  onLogoutClicked() {
    this.accountService.logout();
  }

  onTestApiClicked() {
    this.testApiService.testApi();
  }
}
