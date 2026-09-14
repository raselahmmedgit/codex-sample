import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingIndicatorComponent } from './shared/components/loading-indicator.component';
import { MainLayoutComponent } from './layout/main-layout.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MainLayoutComponent, LoadingIndicatorComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {}
