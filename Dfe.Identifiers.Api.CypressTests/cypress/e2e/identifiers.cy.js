describe('Identifiers endpoint tests', () => {

  const apiKey = Cypress.env('apiKey')
  const baseUrl = Cypress.env('url')
  const ukprn = '10067112'
  const trustReferenceNumber = 'TR03739'

  context('Identifiers endpoint', () => {

    const identifiersEndpoint = 'api/identifier'

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
  })
})
