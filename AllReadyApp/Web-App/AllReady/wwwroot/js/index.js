﻿(function (ko, $, navigator, campaigns) {
    function Campaign(item) {
        for (var prop in item) {
            this[prop] = item[prop];
        }
        this.displayDate = function () {
            var start = moment(this.StartDate).utcOffset(this.StartDate).format("dddd, MMMM Do YYYY");
            var end = moment(this.EndDate).utcOffset(this.EndDate).format("dddd, MMMM Do YYYY");
            return start + ' - ' + end;
        }
        return this;
    }

    function IndexViewModel(campaigns) {
        var list = campaigns.map(function (item) { return new Campaign(item); })
        this.campaigns = ko.observableArray(list);
    }

    ko.applyBindings(new IndexViewModel(campaigns));
})(ko, $, window.navigator, modelCampaigns);