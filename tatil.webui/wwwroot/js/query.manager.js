class Query {
    constructor(queryId, categoryName, filterIds, city, dist, neigh, bedC, humanC, search, page) {
        this.queryLink = document.getElementById(queryId);
        this.filterIds = JSON.parse(filterIds);
        this.city = city;
        this.dist = dist;
        this.neigh = neigh;
        this.bedC = bedC;
        this.humanC = humanC;
        this.search = search;
        this.page = page;
        this.categoryName = this.categoryName
    }

    updateFilterIds(input, filterId, categoryName) {
        const checked = input.checked;
        if (checked) {
            this.filterIds.push(filterId);
        }
        else {
            let index = this.filterIds.indexOf(filterId);
            if (index != -1) {
                this.filterIds.splice(index, 1);
            }
        }
        this.updateQuery(categoryName);
    }

    updateBedInput(input, categoryName) {
        this.bedC = input.value;
        this.updateQuery(categoryName);
    }
    updateHumanInput(input, categoryName) {
        this.humanC = input.value;
        this.updateQuery(categoryName);
    }
    updateSearchQuery(input, categoryName) {
        this.search = input.value;
        this.updateQuery(categoryName);
    }

    updateCity(selectbox, categoryName) {
        this.city = selectbox.options[selectbox.selectedIndex].text
        this.dist = null;
        this.neigh = null;
        this.updateQuery(categoryName);
    }
    updateDisct(selectbox, categoryName) {
        this.dist = selectbox.options[selectbox.selectedIndex].text
        this.neigh = null;
        this.updateQuery(categoryName);
    }
    updateNeigh(selectbox, categoryName) {
        this.neigh = selectbox.options[selectbox.selectedIndex].text
        this.updateQuery(categoryName);
    }
    updatePage(pageC, categoryName) {
        this.page = pageC;
        this.updateQuery(categoryName);
        this.queryLink.click();
    }


    updateQuery(categoryName) {
        this.query = `/${categoryName}?filters=`;
        if (this.filterIds.length > 0) {
            if (this.filterIds.length == 0) {
                this.query = `/${categoryName}?filters=`;
            }
            else {
                this.filterIds.forEach(filterId => {
                    if (filterId != "" & filterId != " " && filterId != null) {
                        this.query += `${filterId}|`
                    }
                });
            }
        }
        if (this.city != null) {
            this.query += `&city=${this.city}`
            if (this.dist != null) {
                this.query += `&district=${this.dist}`
                if (this.neigh != null) {
                    this.query += `&neighborhood=${this.neigh}`
                }
            }
        }
        if (this.bedC != null) {
            this.query += `&bedCapacity=${this.bedC}`;
        }
        if (this.humanC != null) {
            this.query += `&humanCapacity=${this.humanC}`;
        }
        if (this.search != null) {
            this.query += `&search=${this.search}`;
        }
        if (this.page != null) {
            this.query += `&page=${this.page}`;
        }
        this.queryLink.href = this.query
    }

}


