class ApiError extends Error {
  name = 'ApiError'
}

export default class OrdOps {
  async text (url, data) {
    return fetch('http://localhost:3777/' + url, data)
      .then(response => {
        const error = response.headers.get('error')
        if (error !== undefined && error !== null) {
          throw new ApiError(error)
        }
        if (response.status === 401) {
          localStorage.removeItem('token')
          throw new ApiError('401')
        }
        if (response.status > 401) {
          throw new ApiError('AccessError')
        }
        return response.text()
      })
      .catch(error => {
        if (error.name === 'ApiError') throw error
        throw new ApiError('AccessError')
      })
  }

  formateBody (data) {
    if (Object.keys(data).length > 1) {
      return JSON.stringify(data)
    } else {
      return JSON.stringify(data[Object.keys(data)[0]])
    }
  }

  async getOrders (sortByColumn, descending, searchByColumn, search, pageShow, numOfDispLines) {
    debugger
    console.log(search)
    return await this.text('order/GetOrders/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        sortByColumn: sortByColumn,
        descending: descending,
        searchByColumn: searchByColumn,
        searchAction: { String: search.string, Date: search.date, DateA: search.dateA, DateZ: search.dateZ },
        pageShow: pageShow,
        numOfDispLines: numOfDispLines
      })
    })
  }

  async getOrderPositions (orderId) {
    return await this.text('order/GetOrderPositions/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderID: orderId
      })
    })
  }

  async getOrdersNumber (sortByColumn, descending, searchByColumn, search, pageShow, numOfDispLines) {
    debugger
    return await this.text('order/GetOrdersCount/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        sortByColumn: sortByColumn,
        descending: descending,
        searchByColumn: searchByColumn,
        searchAction: { String: search.string, Date: search.date, DateA: search.dateA, DateZ: search.dateZ },
        pageShow: pageShow,
        numOfDispLines: numOfDispLines
      })
    })
  }

  async saveOrder (orderNumber, creationDate, customName, customSurname, customPatronymic, positions) {
    debugger
    return await this.text('order/SetOrder/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderNumber: orderNumber,
        CreationDate: creationDate,
        CustomName: customName,
        CustomSurname: customSurname,
        CustomPatronymic: customPatronymic,
        Positions: positions
      })
    })
  }

  async saveOrderPosition (orderId, productName, productPrice, productSku) {
    return await this.text('order/SetOrderPosition/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderID: orderId,
        ProductName: productName,
        ProductPrice: productPrice,
        ProductSku: productSku
      })
    })
  }

  async editOrder (orderId, orderNumber, creationDate, customName, customSurname, customPatronymic, sumMoney, positions) {
    return await this.text('order/EditOrder/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderID: orderId,
        OrderNumber: orderNumber,
        CreationDate: creationDate,
        CustomName: customName,
        CustomSurname: customSurname,
        CustomPatronymic: customPatronymic,
        SumMoney: sumMoney,
        Positions: positions
      })
    })
  }

  async editOrderPosition (positionId, productName, productPrice, productSku, sumMoney) {
    return await this.text('order/EditOrderPositions/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        PositionID: positionId,
        ProductName: productName,
        ProductPrice: productPrice,
        ProductSku: productSku,
        SumMoney: sumMoney
      })
    })
  }

  async editOrderInfo (orderId) {
    return await this.text('order/editOrderInfo/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderID: orderId
      })
    })
  }

  async editOrderPositionInfo (positionId) {
    return await this.text('order/editOrderInfo/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        PositionID: positionId
      })
    })
  }

  async removeOrder (orderIds) {
    return await this.text('order/RemoveOrder/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        OrderIDs: orderIds
      })
    })
  }

  async removeOrderPosition (positionIds) {
    return await this.text('order/RemoveOrderPositions/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: this.formateBody({
        PositionIDs: positionIds
      })
    })
  }
}
