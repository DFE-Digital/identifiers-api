describe('Identifiers endpoint tests', () => {

  const apiKey = Cypress.env('apiKey')
  const baseUrl = Cypress.env('url')
  const identifiersEndpoint = 'api/identifier'

  context('Trusts', () => {

    const ukprn = '10067112'
    const trustReferenceNumber = 'TR03739'
    const uid = ''

    it.skip('should return a list of identifiers for a Trust Reference Number', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${trustReferenceNumber}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })

    it.skip('should return a list of identifiers for a UKPRN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${ukprn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })

    it.skip('should return a list of identifiers for a UID', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${uid}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })
  })

  context('Establishments', () => {

    const ukprn = ''
    const laestab = ''
    const urn = ''

    it.skip('should return a list of identifiers for a LAESTAB', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${laestab}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })

    it.skip('should return a list of identifiers for a UKPRN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${ukprn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })

    it.skip('should return a list of identifiers for a URN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${urn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
      })
    })
  })
})
