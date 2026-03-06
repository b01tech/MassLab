import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatsCardComponent } from './components/stats-card/stats-card.component';
import { RecentActivitiesComponent } from './components/recent-activities/recent-activities.component';
import { MonthlyGoalComponent } from './components/monthly-goal/monthly-goal.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    StatsCardComponent,
    RecentActivitiesComponent,
    MonthlyGoalComponent,
  ],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {}
