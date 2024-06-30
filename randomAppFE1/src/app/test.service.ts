import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class TestService {
  constructor(private http: HttpClient) {}

  testApi() {
    const url = `${environment.api}/api/test/`;
    this.http.get(url).subscribe((res) => {
      console.log('It is working');
    });
  }
}
