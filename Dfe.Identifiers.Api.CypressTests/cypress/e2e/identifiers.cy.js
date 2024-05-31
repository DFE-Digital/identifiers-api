describe('Identifiers endpoint tests', () => {

  const apiKey = Cypress.env('apiKey')
  const baseUrl = `${Cypress.env('url')}`
  const identifiersEndpoint = 'api/identifier'

  context('Trusts', () => {

    const ukprn = '10067112'
    const trustReferenceNumber = 'TR03739'
    const uid = '16917'

    it('should return a list of identifiers for a Trust Reference Number', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${trustReferenceNumber}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.trusts).to.have.lengthOf.at.most(1)
        expect(response.body.trusts[0]).to.have.keys('uid', 'ukprn', 'trustReference')
        expect(response.body.trusts[0]).to.eql({
          'uid': uid,
          'ukprn': ukprn,
          'trustReference': trustReferenceNumber
        })
        expect(response.body.establishments).to.eql([])
      })
    })

    it('should return a list of identifiers for a UKPRN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${ukprn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.trusts).to.have.lengthOf.at.most(1)
        expect(response.body.trusts[0]).to.have.keys('uid', 'ukprn', 'trustReference')
        expect(response.body.trusts[0]).to.eql({
          'uid': uid,
          'ukprn': ukprn,
          'trustReference': trustReferenceNumber
        })
        expect(response.body.establishments).to.eql([])
      })
    })

    it('should return a list of identifiers for a UID', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${uid}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.trusts).to.have.lengthOf.at.most(1)
        expect(response.body.trusts[0]).to.have.keys('uid', 'ukprn', 'trustReference')
        expect(response.body.trusts[0]).to.eql({
          'uid': uid,
          'ukprn': ukprn,
          'trustReference': trustReferenceNumber
        })
        expect(response.body.establishments).to.eql([])
      })
    })
  })

  context('Establishments', () => {

    const ukprn = '10079319'
    const laestab = '201/3614'
    const urn = '100000'

    it('should return a list of identifiers for a LAESTAB', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${sanitiseLaestab(laestab)}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.establishments).to.have.lengthOf.at.most(1)
        expect(response.body.establishments[0]).to.have.keys('laestab', 'ukprn', 'urn')
        expect(response.body.establishments[0]).to.eql({
          'laestab': laestab,
          'ukprn': ukprn,
          'urn': urn
        })
        expect(response.body.trusts).to.eql([])
      })
    })

    it('should return a list of identifiers for a UKPRN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${ukprn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.establishments).to.have.lengthOf.at.most(1)
        expect(response.body.establishments[0]).to.have.keys('laestab', 'ukprn', 'urn')
        expect(response.body.establishments[0]).to.eql({
          'laestab': laestab,
          'ukprn': ukprn,
          'urn': urn
        })
        expect(response.body.trusts).to.eql([])
      })
    })

    it('should return a list of identifiers for a URN', () => {

      cy.api({
        method: 'GET',
        url: `${baseUrl}/${identifiersEndpoint}/${urn}`,
        headers: {
          ApiKey: apiKey,
          "Content-Type": "application/json"
        }
      }).then((response) => {
        expect(response.status).to.eq(200)
        expect(response.body).to.have.keys('trusts', 'establishments', 'conversionProjects', 'transferProjects', 'formAMatProjects')
        expect(response.body.establishments).to.have.lengthOf.at.most(1)
        expect(response.body.establishments[0]).to.have.keys('laestab', 'ukprn', 'urn')
        expect(response.body.establishments[0]).to.eql({
          'laestab': laestab,
          'ukprn': ukprn,
          'urn': urn
        })
        expect(response.body.trusts).to.eql([])
      })
    })
  })
})

function sanitiseLaestab(laestab) {
  return laestab.replace('/', '-')
}
