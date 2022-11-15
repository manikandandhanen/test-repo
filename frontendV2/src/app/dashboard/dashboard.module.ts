import { LayoutsMaterialModule } from "app/layouts/layouts-material.module";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FlexLayoutModule } from "@angular/flex-layout";
import { NgChartsModule } from "ng2-charts";
import { NgxEchartsModule } from "ngx-echarts";
import { SharedPipesModule } from "app/layouts/pipes/shared-pipes.module";
import { DashboardRoutes } from "./dashboard.routing";
import { AnalyticsComponent } from "./analytics/analytics.component";

@NgModule({
  imports: [
    CommonModule,
    LayoutsMaterialModule,
    FlexLayoutModule,
    NgChartsModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
    SharedPipesModule,
    RouterModule.forChild(DashboardRoutes)
  ],
  declarations: [AnalyticsComponent],
  exports: []
})
export class DashboardModule {}
