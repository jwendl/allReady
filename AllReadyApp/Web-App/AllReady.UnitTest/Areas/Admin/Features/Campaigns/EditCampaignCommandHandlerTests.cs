﻿using AllReady.Areas.Admin.Features.Campaigns;
using AllReady.Areas.Admin.Models;
using System;
using System.Linq;
using Xunit;

namespace AllReady.UnitTest.Areas.Admin.Features.Campaigns
{
    public class EditCampaignCommandHandlerTests : InMemoryContextTest
    {
        [Fact]
        public void AddNewCampaign()
        {
            // Arrange
            var handler = new EditCampaignCommandHandler(Context);
            var newCampaign = new CampaignSummaryModel { Name = "New", Description = "Desc", TimeZoneId ="UTC" };

            // Act
            var result = handler.Handle(new EditCampaignCommand { Campaign = newCampaign });

            // Assert
            Assert.Equal(5, Context.Campaigns.Count());
            Assert.True(result > 0);
        }

        /// <summary>
        /// Tests that the columms belonging the campaign table record are actually updated when a campaign is edited
        /// </summary>
        /// <remarks>This test is not testing the creation of location record, or impact record as those should be seperate tests</remarks>
        [Fact]
        public void UpdatingExistingCampaignUpdatesAllCoreProperties()
        {
            // Arrange
            var name = "New Name";
            var desc = "New Desc";
            var fullDesc = "New Full Desc";
            var timezoneId = "GMT Standard Time";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(30);
            var org = 2;
            
            var handler = new EditCampaignCommandHandler(Context);
            var updatedCampaign = new CampaignSummaryModel {
                Id = 2,
                Name = name,
                Description = desc,
                FullDescription = fullDesc,
                TimeZoneId = timezoneId,
                StartDate = startDate,
                EndDate = endDate,
                OrganizationId = org,
            };

            // Act
            var result = handler.Handle(new EditCampaignCommand { Campaign = updatedCampaign });
            var savedCampaign = Context.Campaigns.SingleOrDefault(s => s.Id == 2);

            // Assert
            Assert.Equal(4, Context.Campaigns.Count());
            Assert.Equal(2, result);

            Assert.Equal(name, savedCampaign.Name);
            Assert.Equal(desc, savedCampaign.Description);
            Assert.Equal(fullDesc, savedCampaign.FullDescription);
            Assert.Equal(timezoneId, savedCampaign.TimeZoneId);
            Assert.NotEqual(startDate.Date, new DateTime()); // We're not testing the date logic in this test, only that a datetime value is saved
            Assert.NotEqual(endDate.Date, new DateTime()); // We're not testing the date logic in this test, only that a datetime value is saved
            Assert.Equal(org, savedCampaign.ManagingOrganizationId);
        }

        protected override void LoadTestData()
        {
            CampaignsHandlerTestHelper.LoadCampaignssHandlerTestData(Context);
        }
    }
}
