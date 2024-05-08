#Introduction
To ensure a consistent and structured approach for providing and sharing clinical information, the PCEHR system accepts only documents of supported HL7® Clinical Document Architecture (CDA®) structured document types.1

HL7® CDA® documents are XML documents deriving their machine-processable meaning from the HL7® Reference Information Model (RIM). They use the HL7® Version 3 Data Types.

The Clinical Document Data Extractor Library (previously called the CDA Extractor Library) uses the NEHTA CDA® Schema Extension and the relevant structured content specifications and CDA® implementation guides to extract the atomic data from level 3A and 3B CDA® documents built against the NEHTA specifications.

# Content
The Clinical Document Data Extractor Library simplifies the development process by providing vendors with a simple interface for extracting data from a range of CDA® documents. The library uses the information from NEHTA-developed CDA® implementation guides. The Clinical Document Data Extractor Library can extract data from the following CDA® documents:

## Continuity of Care
- eReferral
- Discharge Summary
- Event Summary
- Shared Health Summary
- Specialist Letter

## Prescription and Dispense
- eHealth Prescription Record (previously called PCEHR Prescription Record)
- eHealth Dispense Record (previously called PCEHR Dispense Record)

The Clinical Document Data Extractor Library has the ability to extract data from the following sections within each of the CDA® documents listed. The data extracted is then presented back in a consistent format.

| Sections | SHS | ES | REF | SL | DS | PR | DR |
|---|---|---|---|---|---|---|---|
| Adverse Reactions | X | X | X | X | X |   |   |
| Medications | X | X | X | X | X | X | X |
| Medical History | X | X | X | X | X |   |   |
| Immunisations | X | X |   |   |   |   |   |


# Project
This is a dotNet software library that aims to help extract structured data from Agency defined CDA documents.

# Setup
- To build the distributable package, Visual Studio must be installed.
- Start up CdaExtractor.sln

# Solution
The solution consists of several projects, however the main project is the CDAExtacror project. 
This project contains the code to generate and return a model with the extracted data in it.

# Building and using the library
The solution can be built using 'F6'. 

# Library Usage
Documentation can be found in the sample project.

# Licensing
See [LICENSE](LICENSE.txt) file.